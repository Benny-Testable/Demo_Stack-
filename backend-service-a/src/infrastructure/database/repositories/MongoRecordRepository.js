const { RecordModel } = require('../mongoose/schemas/Record.schema');
const { RecordEntity } = require('../../../domain/entities/Record.entity');

class MongoRecordRepository {
  async save(recordEntity) {
    const doc = await RecordModel.create({
      title: recordEntity.title,
      description: recordEntity.description,
    });
    return new RecordEntity({
      id: doc._id.toString(),
      title: doc.title,
      description: doc.description,
      createdAt: doc.createdAt,
    });
  }

  async findById(id) {
    const doc = await RecordModel.findById(id).catch(() => null);
    if (!doc) return null;
    return new RecordEntity({
      id: doc._id.toString(),
      title: doc.title,
      description: doc.description,
      createdAt: doc.createdAt,
    });
  }

  async findAll(limit = 200) {
    const docs = await RecordModel.find().sort({ createdAt: -1 }).limit(limit);
    return docs.map(
      (doc) =>
        new RecordEntity({
          id: doc._id.toString(),
          title: doc.title,
          description: doc.description,
          createdAt: doc.createdAt,
        }),
    );
  }
}

module.exports = { MongoRecordRepository };
