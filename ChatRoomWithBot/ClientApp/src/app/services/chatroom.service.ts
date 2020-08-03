import { EventEmitter, Inject, Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';  
import { ChatMessageModel } from '../models/chatmessage.model';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
  
@Injectable()  
export class ChatRoomService {  
  messageReceived = new EventEmitter<ChatMessageModel>();  
  connectionEstablished = new EventEmitter<Boolean>();  

  userManager: UserManager;

  private connectionIsEstablished = false;  
  private _hubConnection: HubConnection;
  private _baseUrl: string;
  
  constructor(@Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();  
  }  

  private getAccessToken() {
    this.userManager.getUser().then((user: User) => {
      if (user && user.access_token) {
        return user.access_token;
      } else if (user) {
        return this.userManager.signinSilent().then((user: User) => {
          return user.access_token;
        });
      } else {
        throw new Error('user is not logged in');
      }
    });
}


  sendMessage(message: ChatMessageModel) {  
    this._hubConnection.invoke('NewMessage', message);  
  }  
  
  private createConnection() {
    var token = this.getAccessToken.toString();
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
