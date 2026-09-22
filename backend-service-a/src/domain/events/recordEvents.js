const EventEmitter = require('events');

/**
 * Domain Event Emitter: In-process publisher for cross-service gRPC streaming
 */
const recordEvents = new EventEmitter();
recordEvents.setMaxListeners(100);

module.exports = { recordEvents };
