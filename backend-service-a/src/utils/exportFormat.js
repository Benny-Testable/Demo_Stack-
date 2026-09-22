/**
 * Export Formatter Utility
 * Provides CSV, JSON, and NDJSON transformation logic.
 * Cross-service identical block for jscpd and SonarJS clone detection.
 */

function formatRecordToCsvRow(record) {
  if (!record || typeof record !== 'object') return '';
  const id = String(record.id || '').replace(/"/g, '""');
  const title = String(record.title || '').replace(/"/g, '""');
  const desc = String(record.description || '').replace(/"/g, '""');
  const createdAt = String(record.createdAt || '');

  return `"${id}","${title}","${desc}","${createdAt}"`;
}

function exportRecordsToCsv(records) {
  if (!Array.isArray(records) || records.length === 0) {
    return 'id,title,description,createdAt\n';
  }
  const header = 'id,title,description,createdAt\n';
  const rows = records.map((r) => formatRecordToCsvRow(r)).join('\n');
  return header + rows;
}

function exportRecordsToNdjson(records) {
  if (!Array.isArray(records) || records.length === 0) return '';
  return records
    .filter((r) => r && typeof r === 'object')
    .map((r) => JSON.stringify(r))
    .join('\n');
}

module.exports = {
  formatRecordToCsvRow,
  exportRecordsToCsv,
  exportRecordsToNdjson,
};
