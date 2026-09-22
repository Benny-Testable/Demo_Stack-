/**
 * Configuration Layer: Environment validation and settings
 */
const envConfig = {
  port: process.env.PORT || 3001,
  grpcPort: process.env.GRPC_PORT || 50051,
  mongoUri: process.env.MONGO_URI || 'mongodb://localhost:27017/ceplatform',
  nodeEnv: process.env.NODE_ENV || 'development',
};

module.exports = { envConfig };
