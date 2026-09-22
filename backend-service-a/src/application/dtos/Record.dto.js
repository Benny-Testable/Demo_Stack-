/**
 * Application DTO: RecordDto
 */
class RecordDto {
  constructor({ id, title, description, createdAt }) {
    this.id = id;
    this.title = title;
    this.description = description;
    this.createdAt = createdAt;
  }

  static fromEntity(entity) {
    if (!entity) return null;
    return new RecordDto(entity.toWire ? entity.toWire() : entity);
  }
}

module.exports = { RecordDto };
