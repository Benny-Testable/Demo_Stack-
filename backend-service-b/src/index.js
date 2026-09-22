const { createApp } = require('./app');
const { envConfig } = require('./config/env.config');
const { ElasticsearchAdapter } = require('./infrastructure/search/ElasticsearchAdapter');
const { SnsEventPublisher } = require('./infrastructure/messaging/SnsEventPublisher');
const { SesEmailNotifier } = require('./infrastructure/messaging/SesEmailNotifier');
const { GrpcRecordConsumer } = require('./infrastructure/grpc/GrpcRecordConsumer');
const { ProcessIncomingRecordUseCase } = require('./application/use-cases/RecordUseCases');

const esAdapter = new ElasticsearchAdapter();
const snsPublisher = new SnsEventPublisher();
const sesNotifier = new SesEmailNotifier();

const processRecordUseCase = new ProcessIncomingRecordUseCase({
  elasticsearchAdapter: esAdapter,
  snsEventPublisher: snsPublisher,
  sesEmailNotifier: sesNotifier,
});

async function handleNewRecord(record) {
  try {
    await processRecordUseCase.execute(record);
  } catch (err) {
    console.error('[service-b] failed to process record', record.id, err.message);
  }
}

async function bootstrap() {
  await esAdapter.ensureIndex();
  await snsPublisher.ensureTopic();

  const app = createApp();
  app.listen(envConfig.port, () => {
    console.log(`[service-b][rest] listening on :${envConfig.port}`);
  });

  const grpcClient = GrpcRecordConsumer.createClient();
  GrpcRecordConsumer.watchRecords(grpcClient, handleNewRecord);
}

if (require.main === module) {
  bootstrap().catch((err) => {
    console.error('[service-b] fatal startup error', err);
    process.exit(1);
  });
}

module.exports = { bootstrap, handleNewRecord };
