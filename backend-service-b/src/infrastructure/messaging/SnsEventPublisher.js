const { SNSClient, PublishCommand, CreateTopicCommand } = require('@aws-sdk/client-sns');
const { envConfig } = require('../../config/env.config');

const TOPIC_NAME = 'ce-records';

class SnsEventPublisher {
  constructor(endpoint = envConfig.awsEndpoint, region = envConfig.awsRegion) {
    this.client = new SNSClient({
      endpoint,
      region,
      credentials: { accessKeyId: 'test', secretAccessKey: 'test' },
    });
    this.topicArn = envConfig.snsTopicArn;
  }

  async ensureTopic() {
    try {
      const res = await this.client.send(new CreateTopicCommand({ Name: TOPIC_NAME }));
      if (res.TopicArn) {
        this.topicArn = res.TopicArn;
        console.log(`[service-b][sns] topic ready: ${this.topicArn}`);
      }
    } catch (err) {
      console.warn(`[service-b][sns] ensureTopic warning: ${err.message}`);
    }
  }

  async publishRecordCreated(record) {
    return this.client.send(
      new PublishCommand({
        TopicArn: this.topicArn,
        Subject: `Record Created: ${record.title}`,
        Message: JSON.stringify(record),
      }),
    );
  }
}

module.exports = { SnsEventPublisher, TOPIC_NAME };
