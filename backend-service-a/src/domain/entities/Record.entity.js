/**
 * Domain Entity: Record
 * Pure domain model encapsulating core entity rules and wire serialization.
 */
class RecordEntity {
  constructor({ id, title, description = '', createdAt = new Date().toISOString() }) {
    if (!title || typeof title !== 'string' || !title.trim()) {
      throw new Error('Record title is mandatory');
    }
    this.id = String(id || '');
    this.title = title.trim();
    this.description = String(description || '');
    this.createdAt = createdAt instanceof Date ? createdAt.toISOString() : String(createdAt);
  }

  toWire() {
    return {
      id: this.id,
      title: this.title,
      description: this.description,
      createdAt: this.createdAt,
    };
  }
}

module.exports = { RecordEntity };
