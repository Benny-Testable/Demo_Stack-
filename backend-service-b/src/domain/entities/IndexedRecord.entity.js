/**
 * Domain Entity: IndexedRecord
 */
class IndexedRecordEntity {
  constructor({ id, title, description = '', createdAt = new Date().toISOString() }) {
    this.id = String(id || '');
    this.title = String(title || '');
    this.description = String(description || '');
    this.createdAt = String(createdAt || '');
  }
}

module.exports = { IndexedRecordEntity };
