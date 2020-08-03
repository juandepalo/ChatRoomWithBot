import { EventEmitter, Inject, Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';  
import { ChatMessageModel } from '../models/chatmessage.model';  
  
@Injectable()  
export class ChatRoomService {  
  messageReceived = new EventEmitter<ChatMessageModel>();  
  connectionEstablished = new EventEmitter<Boolean>();  
  
  private connectionIsEstablished = false;  
  private _hubConnection: HubConnection;
  private _baseUrl: string;
  
  constructor(@Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();  
  }  
  
  sendMessage(message: ChatMessageModel) {  
    this._hubConnection.invoke('NewMessage', message);  
  }  
  
  private createConnection() {  
    this._hubConnection = new HubConnectionBuilder()  
      .withUrl(this._baseUrl + 'chatroom-events')  
      .build();  
  }  
  
  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {  
        this.connectionIsEstablished = true;  
        console.log('Hub connection started');  
        this.connectionEstablished.emit(true);  
      })  
      .catch(err => {  
        console.log('Error while establishing connection, retrying...');  
        setTimeout(function () { this.startConnection(); }, 5000);  
      });  
  }  
  
  private registerOnServerEvents(): void {  
    this._hubConnection.on('Send', (data: any) => {  
      this.messageReceived.emit(data);  
    });  
  }  
}  
