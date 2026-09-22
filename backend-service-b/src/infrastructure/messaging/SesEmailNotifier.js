const { SESClient, SendEmailCommand } = require('@aws-sdk/client-ses');
const { envConfig } = require('../../config/env.config');

class SesEmailNotifier {
  constructor(endpoint = envConfig.awsEndpoint, region = envConfig.awsRegion) {
    this.client = new SESClient({
      endpoint,
      region,
      credentials: { accessKeyId: 'test', secretAccessKey: 'test' },
    });
    this.fromEmail = envConfig.sesFromEmail;
    this.toEmail = envConfig.sesToEmail;
  }

  async sendRecordIndexedEmail(record) {
    const params = {
      Source: this.fromEmail,
      Destination: { ToAddresses: [this.toEmail] },
      Message: {
        Subject: { Data: `[CE-Platform] New Record Indexed: ${record.title}` },
        Body: {
          Text: {
            Data: `Record ${record.id} ("${record.title}") has been indexed into Elasticsearch.\n\nDescription: ${record.description}\nCreated: ${record.createdAt}`,
          },
        },
      },
    };
    return this.client.send(new SendEmailCommand(params));
  }
}

module.exports = { SesEmailNotifier };
