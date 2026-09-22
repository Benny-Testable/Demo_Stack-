const mongoose = require('mongoose');

const recordSchema = new mongoose.Schema(
  {
    title: { type: String, required: true, trim: true },
    description: { type: String, default: '' },
  },
  {
    timestamps: true,
  },
);

recordSchema.methods.toWire = function () {
  return {
    id: this._id.toString(),
    title: this.title,
    description: this.description,
    createdAt: this.createdAt ? this.createdAt.toISOString() : new Date().toISOString(),
  };
};

const RecordModel = mongoose.models.Record || mongoose.model('Record', recordSchema);

module.exports = { RecordModel };
