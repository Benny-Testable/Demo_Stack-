/**
 * Configuration Layer: Backend Service B
 */
const envConfig = {
  port: process.env.PORT || 3002,
  grpcTarget: process.env.GRPC_TARGET || '127.0.0.1:50051',
  elasticNode: process.env.ELASTIC_NODE || 'http://127.0.0.1:9200',
  awsEndpoint: process.env.AWS_ENDPOINT || 'http://127.0.0.1:4566',
  awsRegion: process.env.AWS_REGION || 'us-east-1',
  snsTopicArn: process.env.SNS_TOPIC_ARN || 'arn:aws:sns:us-east-1:000000000000:ce-records',
  sesFromEmail: process.env.SES_FROM_EMAIL || 'notifications@ceplatform.local',
  sesToEmail: process.env.SES_TO_EMAIL || 'admin@ceplatform.local',
};

module.exports = { envConfig };
