const { Client } = require('@elastic/elasticsearch');
const { envConfig } = require('../../config/env.config');

const INDEX_NAME = 'ce-records';

class ElasticsearchAdapter {
  constructor(node = envConfig.elasticNode) {
    this.client = new Client({ node });
    this.indexName = INDEX_NAME;
  }

  async ensureIndex() {
    try {
      const exists = await this.client.indices.exists({ index: this.indexName });
      if (!exists) {
        await this.client.indices.create({
          index: this.indexName,
          mappings: {
            properties: {
              id: { type: 'keyword' },
              title: { type: 'text' },
              description: { type: 'text' },
              createdAt: { type: 'date' },
            },
          },
        });
        console.log(`[service-b][es] created index '${this.indexName}'`);
      }
    } catch (err) {
      console.warn(`[service-b][es] ensureIndex warning: ${err.message}`);
    }
  }

  async indexRecord(record) {
    return this.client.index({
      index: this.indexName,
      id: record.id,
      document: {
        id: record.id,
        title: record.title,
        description: record.description,
        createdAt: record.createdAt,
      },
    });
  }

  async searchRecords(query) {
    const result = await this.client.search({
      index: this.indexName,
      query: {
        multi_match: {
          query,
          fields: ['title^2', 'description'],
        },
      },
    });
    return (result.hits.hits || []).map((h) => h._source);
  }
}

module.exports = { ElasticsearchAdapter, INDEX_NAME };
