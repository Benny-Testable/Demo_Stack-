const path = require('path');
const grpc = require('@grpc/grpc-js');
const protoLoader = require('@grpc/proto-loader');
const { envConfig } = require('../../config/env.config');

const PROTO_PATH = path.resolve(__dirname, '../../../../shared/proto/record.proto');

const packageDefinition = protoLoader.loadSync(PROTO_PATH, {
  keepCase: false,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});

const proto = grpc.loadPackageDefinition(packageDefinition).ceplatform;

class GrpcRecordConsumer {
  static createClient(target = envConfig.grpcTarget) {
    return new proto.RecordService(target, grpc.credentials.createInsecure());
  }

  static watchRecords(client, onRecord) {
    console.log('[service-b][grpc] subscribing to WatchRecords stream...');
    const call = client.WatchRecords({});

    call.on('data', (record) => {
      onRecord(record).catch((err) => {
        console.error('[service-b][grpc] error processing streamed record', err.message);
      });
    });

    call.on('error', (err) => {
      console.warn(`[service-b][grpc] stream error (code ${err.code}): ${err.message}. Retrying in 5s...`);
      setTimeout(() => GrpcRecordConsumer.watchRecords(client, onRecord), 5000);
    });

    call.on('end', () => {
      console.log('[service-b][grpc] stream ended by server. Reconnecting in 5s...');
      setTimeout(() => GrpcRecordConsumer.watchRecords(client, onRecord), 5000);
    });

    return call;
  }
}

module.exports = { GrpcRecordConsumer, proto, PROTO_PATH };
