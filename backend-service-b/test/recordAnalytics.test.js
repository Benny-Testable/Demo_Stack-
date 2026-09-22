const assert = require('assert');
const { summarizeRecordBatch } = require('../src/utils/recordAnalytics');
const { exportRecordsToCsv, exportRecordsToNdjson } = require('../src/utils/exportFormat');

describe('recordAnalytics & exportFormat utility (service-b)', () => {
  it('handles empty batches cleanly', () => {
    const res = summarizeRecordBatch([]);
    assert.strictEqual(res.totalRecords, 0);
    assert.strictEqual(res.status, 'EMPTY_BATCH');
  });

  it('summarizes batches and categorizes items correctly', () => {
    const mockRecords = [
      { id: '101', title: 'Student Enrollment Record', description: 'Course registration' },
      { id: '102', title: 'Payment Receipt', description: 'Retail order transaction' },
    ];
    const res = summarizeRecordBatch(mockRecords);
    assert.strictEqual(res.totalRecords, 2);
    assert.strictEqual(res.categories.edtech, 1);
    assert.strictEqual(res.categories.retail, 1);
  });

  it('exports records to CSV format', () => {
    const records = [{ id: '1', title: 'T1', description: 'D1', createdAt: '2026-09-22' }];
    const csv = exportRecordsToCsv(records);
    assert.ok(csv.startsWith('id,title,description,createdAt\n'));
    assert.ok(csv.includes('"1","T1","D1","2026-09-22"'));
  });

  it('exports records to NDJSON format', () => {
    const records = [{ id: '1', title: 'T1' }, { id: '2', title: 'T2' }];
    const ndjson = exportRecordsToNdjson(records);
    const lines = ndjson.split('\n');
    assert.strictEqual(lines.length, 2);
  });
});
