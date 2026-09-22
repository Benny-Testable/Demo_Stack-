const assert = require('assert');
const { SnsEventPublisher, TOPIC_NAME } = require('../src/infrastructure/messaging/SnsEventPublisher');
const { SesEmailNotifier } = require('../src/infrastructure/messaging/SesEmailNotifier');
const { ElasticsearchAdapter, INDEX_NAME } = require('../src/infrastructure/search/ElasticsearchAdapter');
const { envConfig } = require('../src/config/env.config');

describe('integration client defaults (service-b)', () => {
  it('SNS defaults to the localstack endpoint and the real topic name', () => {
    const publisher = new SnsEventPublisher();
    assert.strictEqual(envConfig.awsEndpoint, 'http://127.0.0.1:4566');
    assert.strictEqual(TOPIC_NAME, 'ce-records');
    assert.ok(publisher);
  });

  it('SES defaults to the localstack endpoint', () => {
    const notifier = new SesEmailNotifier();
    assert.strictEqual(envConfig.awsEndpoint, 'http://127.0.0.1:4566');
    assert.ok(notifier);
  });

  it('Elasticsearch defaults to the local cluster and the real index name', () => {
    const es = new ElasticsearchAdapter();
    assert.strictEqual(envConfig.elasticNode, 'http://127.0.0.1:9200');
    assert.strictEqual(INDEX_NAME, 'ce-records');
    assert.ok(es);
  });
});
