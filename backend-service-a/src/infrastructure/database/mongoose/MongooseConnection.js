const mongoose = require('mongoose');

class MongooseConnection {
  static async connect(mongoUri) {
    if (mongoose.connection.readyState === 1) return mongoose.connection;
    return mongoose.connect(mongoUri);
  }

  static async disconnect() {
    if (mongoose.connection.readyState !== 0) {
      return mongoose.disconnect();
    }
  }
}

module.exports = { MongooseConnection };
