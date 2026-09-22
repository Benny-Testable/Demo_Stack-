const { createApp } = require('./app');
const { envConfig } = require('./config/env.config');
const { MongooseConnection } = require('./infrastructure/database/mongoose/MongooseConnection');
const { startGrpcServer } = require('./infrastructure/grpc/GrpcRecordServer');

async function bootstrap() {
  await MongooseConnection.connect(envConfig.mongoUri);
  console.log('[service-a][db] connected to MongoDB');

  await startGrpcServer(envConfig.grpcPort);

  const app = createApp();
  app.listen(envConfig.port, () => {
    console.log(`[service-a][rest] listening on :${envConfig.port}`);
  });
}

if (require.main === module) {
  bootstrap().catch((err) => {
    console.error('[service-a] fatal startup error', err);
    process.exit(1);
  });
}

module.exports = { bootstrap };
