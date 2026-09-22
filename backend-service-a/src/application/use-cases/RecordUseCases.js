const { RecordEntity } = require('../../domain/entities/Record.entity');
const { RecordDto } = require('../dtos/Record.dto');

class CreateRecordUseCase {
  constructor({ recordRepository, eventEmitter }) {
    this.recordRepository = recordRepository;
    this.eventEmitter = eventEmitter;
  }

  async execute({ title, description }) {
    const entity = new RecordEntity({ title, description });
    const saved = await this.recordRepository.save(entity);
    const wire = saved.toWire();

    if (this.eventEmitter) {
      this.eventEmitter.emit('record:created', wire);
    }

    return RecordDto.fromEntity(saved);
  }
}

class GetRecordByIdUseCase {
  constructor({ recordRepository }) {
    this.recordRepository = recordRepository;
  }

  async execute(id) {
    const record = await this.recordRepository.findById(id);
    if (!record) return null;
    return RecordDto.fromEntity(record);
  }
}

class ListRecordsUseCase {
  constructor({ recordRepository }) {
    this.recordRepository = recordRepository;
  }

  async execute(limit = 200) {
    const records = await this.recordRepository.findAll(limit);
    return records.map((r) => RecordDto.fromEntity(r));
  }
}

class ExportFormatUseCase {
  static formatRecordToCsvRow(record) {
    if (!record || typeof record !== 'object') return '';
    const id = String(record.id || '').replace(/"/g, '""');
    const title = String(record.title || '').replace(/"/g, '""');
    const desc = String(record.description || '').replace(/"/g, '""');
    const createdAt = String(record.createdAt || '');

    return `"${id}","${title}","${desc}","${createdAt}"`;
  }

  static exportRecordsToCsv(records) {
    if (!Array.isArray(records) || records.length === 0) {
      return 'id,title,description,createdAt\n';
    }
    const header = 'id,title,description,createdAt\n';
    const rows = records.map((r) => ExportFormatUseCase.formatRecordToCsvRow(r)).join('\n');
    return header + rows;
  }

  static exportRecordsToNdjson(records) {
    if (!Array.isArray(records) || records.length === 0) return '';
    return records
      .filter((r) => r && typeof r === 'object')
      .map((r) => JSON.stringify(r))
      .join('\n');
  }
}

module.exports = {
  CreateRecordUseCase,
  GetRecordByIdUseCase,
  ListRecordsUseCase,
  ExportFormatUseCase,
};
