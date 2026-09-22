class ProcessIncomingRecordUseCase {
  constructor({ elasticsearchAdapter, snsEventPublisher, sesEmailNotifier }) {
    this.elasticsearchAdapter = elasticsearchAdapter;
    this.snsEventPublisher = snsEventPublisher;
    this.sesEmailNotifier = sesEmailNotifier;
  }

  async execute(record) {
    console.log(`[service-b][use-case] processing record ${record.id} (${record.title})`);
    await this.elasticsearchAdapter.indexRecord(record);
    await this.snsEventPublisher.publishRecordCreated(record);
    await this.sesEmailNotifier.sendRecordIndexedEmail(record);
    return { status: 'PROCESSED', recordId: record.id };
  }
}

class SearchRecordsUseCase {
  constructor({ elasticsearchAdapter }) {
    this.elasticsearchAdapter = elasticsearchAdapter;
  }

  async execute(query) {
    return this.elasticsearchAdapter.searchRecords(query);
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
  ProcessIncomingRecordUseCase,
  SearchRecordsUseCase,
  ExportFormatUseCase,
};
