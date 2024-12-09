import { WebSocketServer } from 'ws';

class WebSocketChatServer {
  constructor(port) {
    this.server = new WebSocketServer({ port });
    this.server.on('connection', this.handleConnection.bind(this));
  }

  /**
   * Обробка нового підключення клієнта
   * @param {object} connection 
   */
  handleConnection(connection) {
    connection.on('error', this.handleError);
    connection.on('message', (data) => this.handleMessage(data, connection));
  }

  /**
   * Обробка помилок з'єднання
   * @param {object} error 
   */
  handleError(error) {
    console.error(error);
  }

  /**
   * Обробка вхідних повідомлень від клієнта
   * @param {string} data 
   * @param {object} connection 
   */
  handleMessage(data, connection) {
    console.log('received: %s', data);
    let parsedData;

    try {
      parsedData = JSON.parse(data);
    } catch {
      return connection.send(this.newError('failed to parse'));
    }

    if (!this.validateMessage(parsedData))
      return connection.send(this.newError('failed to validate'));

    switch (parsedData.type) {
      case 'message':
        this.processMessage(parsedData, connection);
        break;
      default:
        connection.send(this.newError('unknown message type'));
    }
  }

  /**
   * Валідація повідомлення
   * @param {object} message 
   * @returns {bool}
   */
  validateMessage(message) {
    return message.hasOwnProperty('type') && message.hasOwnProperty('payload');
  }

  /**
   * Обробка повідомлення
   * @param {object} parsedData 
   * @param {object} connection 
   */
  processMessage(parsedData, connection) {
    const payload = parsedData.payload;

    if (!payload.nickname || !payload.message)
      return connection.send(this.newError('failed to validate'));

    const dateFormatted = new Date(parseInt(payload.timestamp)).toLocaleTimeString();

    this.server.clients.forEach((client) => {
      client.send(this.newMessage(payload.nickname, payload.message, dateFormatted));
    });
  }

  /**
   * Формування нового повідомлення
   * @param {string} username 
   * @param {string} message 
   * @param {string} timestamp 
   * @returns {string}
   */
  newMessage(username, message, timestamp) {
    return JSON.stringify({
      type: 'message',
      payload: { nickname: username, message, timestamp }
    });
  }

  /**
   * Формування повідомлення про помилку
   * @param {string} message 
   * @returns {string}
   */
  newError(message) {
    return JSON.stringify({
      type: 'error',
      payload: { message }
    });
  }
}

// Ініціалізація WebSocket сервера на порту 8080
const chatServer = new WebSocketChatServer(8080);
