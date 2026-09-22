const assert = require('assert');
const {
  RecordAnalyticsDomainService,
} = require('../src/domain/services/RecordAnalyticsDomainService');
const {
  ExportFormatUseCase,
} = require('../src/application/use-cases/RecordUseCases');

describe('RecordAnalyticsDomainService & ExportFormatUseCase (service-a)', () => {
  it('handles empty batches and invalid inputs cleanly', () => {
    const res = RecordAnalyticsDomainService.summarizeRecordBatch([]);
    assert.strictEqual(res.totalRecords, 0);
    assert.strictEqual(res.status, 'EMPTY_BATCH');

    const nullRes = RecordAnalyticsDomainService.summarizeRecordBatch(null);
    assert.strictEqual(nullRes.totalRecords, 0);
  });

  it('summarizes batches and categorizes EdTech vs Retail items', () => {
    const mockRecords = [
      { id: '1', title: 'Student Grade Submission', description: 'Course exam results for algebra' },
      { id: '2', title: 'Payment Card Checkout', description: 'Order cart checkout with visa card' },
      { id: '3', title: 'Short', description: 'Generic system ping' },
    ];

    const res = RecordAnalyticsDomainService.summarizeRecordBatch(mockRecords);
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
    const res = RecordAnalyticsDomainService.summarizeRecordBatch(records, { deepMatrixScan: true });
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

    const res = await RecordAnalyticsDomainService.detectQueryBottlenecks(['rec-1', 'rec-2'], mockDb);
    assert.strictEqual(res.queriedCount, 2);
    assert.strictEqual(res.singleQueryCount, 2);
    assert.deepStrictEqual(res.recommendedBatchQuery, { _id: { $in: ['rec-1', 'rec-2'] } });
  });

  it('evaluateMemoryUsage completes loop iterations', () => {
    const res = RecordAnalyticsDomainService.evaluateMemoryUsage(20);
    assert.strictEqual(res.iterations, 20);
    assert.strictEqual(res.totalAllocatedBytes, 20 * 1024);
  });

  it('ExportFormatUseCase formats CSV and NDJSON correctly', () => {
    const records = [{ id: '1', title: 'Test 1', description: 'Desc 1', createdAt: '2026-09-22' }];
    const csv = ExportFormatUseCase.exportRecordsToCsv(records);
    assert.ok(csv.startsWith('id,title,description,createdAt\n'));
    assert.ok(csv.includes('"1","Test 1","Desc 1","2026-09-22"'));

    const ndjson = ExportFormatUseCase.exportRecordsToNdjson(records);
    assert.ok(ndjson.includes('Test 1'));
  });
});
