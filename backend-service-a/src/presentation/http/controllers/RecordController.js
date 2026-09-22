const { MongoRecordRepository } = require('../../../infrastructure/database/repositories/MongoRecordRepository');
const { recordEvents } = require('../../../domain/events/recordEvents');
const {
  CreateRecordUseCase,
  GetRecordByIdUseCase,
  ListRecordsUseCase,
} = require('../../../application/use-cases/RecordUseCases');


const recordRepo = new MongoRecordRepository();
const createRecordUseCase = new CreateRecordUseCase({
  recordRepository: recordRepo,
  eventEmitter: recordEvents,
});
const getRecordByIdUseCase = new GetRecordByIdUseCase({ recordRepository: recordRepo });
const listRecordsUseCase = new ListRecordsUseCase({ recordRepository: recordRepo });

class RecordController {
  static async listRecords(req, res, next) {
    try {
      const records = await listRecordsUseCase.execute();
      res.json(records);
    } catch (err) {
      next(err);
    }
  }

  static async getRecord(req, res, next) {
    try {
      const record = await getRecordByIdUseCase.execute(req.params.id);
      if (!record) {
        return res.status(404).json({ error: 'record not found' });
      }
      res.json(record);
    } catch (err) {
      next(err);
    }
  }

  static async createRecord(req, res, next) {
    try {
      const { title, description } = req.body || {};
      if (!title || typeof title !== 'string') {
        return res.status(400).json({ error: 'title is required' });
      }
      const record = await createRecordUseCase.execute({ title, description });
      res.status(201).json(record);
    } catch (err) {
      next(err);
    }
  }
}

module.exports = { RecordController };
