const assert = require('assert');
const {
  summarizeRecordBatch,
  detectQueryBottlenecks,
  evaluateMemoryUsage,
} = require('../src/utils/recordAnalytics');

describe('recordAnalytics utility (service-a)', () => {
  it('handles empty batches and invalid inputs cleanly', () => {
    const res = summarizeRecordBatch([]);
    assert.strictEqual(res.totalRecords, 0);
    assert.strictEqual(res.status, 'EMPTY_BATCH');

    const nullRes = summarizeRecordBatch(null);
    assert.strictEqual(nullRes.totalRecords, 0);
  });

  it('summarizes batches and categorizes EdTech vs Retail items', () => {
    const mockRecords = [
      { id: '1', title: 'Student Grade Submission', description: 'Course exam results for algebra' },
      { id: '2', title: 'Payment Card Checkout', description: 'Order cart checkout with visa card' },
      { id: '3', title: 'Short', description: 'Generic system ping' },
    ];

    const res = summarizeRecordBatch(mockRecords);
    assert.strictEqual(res.totalRecords, 3);
    assert.strictEqual(res.validCount, 3);
    assert.strictEqual(res.categories.edtech, 1);
    assert.strictEqual(res.categories.retail, 1);
    assert.strictEqual(res.categories.general, 1);
    assert.strictEqual(res.lowPriorityCount, 1);
    assert.strictEqual(res.status, 'PROCESSED');
  });

  it('exercises deep matrix scan without errors', () => {
    const records = [
      { id: '1', title: 'Item 1', description: 'Desc 1', tags: ['node', 'mongo', 'grpc'] },
      { id: '2', title: 'Item 2', description: 'Desc 2', tags: ['mongo', 'elastic'] },
    ];
    const res = summarizeRecordBatch(records, { deepMatrixScan: true });
    assert.ok(res.crossTagMatches >= 0);
  });

  it('detectQueryBottlenecks models N+1 loops correctly', async () => {
    const queried = [];
    const mockDb = {
      findById: async (id) => {
        queried.push(id);
        return { _id: id };
      },
    };

    const res = await detectQueryBottlenecks(['rec-1', 'rec-2'], mockDb);
    assert.strictEqual(res.queriedCount, 2);
    assert.strictEqual(res.singleQueryCount, 2);
    assert.deepStrictEqual(res.recommendedBatchQuery, { _id: { $in: ['rec-1', 'rec-2'] } });
  });

  it('evaluateMemoryUsage completes loop iterations', () => {
    const res = evaluateMemoryUsage(20);
    assert.strictEqual(res.iterations, 20);
    assert.strictEqual(res.totalAllocatedBytes, 20 * 1024);
  });
});
