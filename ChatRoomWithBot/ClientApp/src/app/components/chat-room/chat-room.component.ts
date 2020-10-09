import { Component, NgZone, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ChatMessageModel } from '../../models/chatmessage.model';
import { ChatRoomService } from '../../services/chatroom.service';

@Component({
  selector: 'chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.css']
})
export class ChatRoomComponent implements OnInit {
  public chatMessages: ChatMessageModel[];
  public msgForm: FormGroup;
  public msgItem: ChatMessageModel;
  private _baseUrl: string;
  public currentDT: Date;
  public chatLimit: number = 50;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private chatRoomService: ChatRoomService,
    private _ngZone: NgZone
  ) {
    this._baseUrl = baseUrl;

    this.currentDT = new Date();

    this.currentDT.setDate(this.currentDT.getDate() - 1);

    console.log(this.currentDT)
    this.subscribeToEvents();

    http.get<ChatMessageModel[]>(this._baseUrl + 'api/chats/getmessages').subscribe(result => {
      this.chatMessages = result;
    }, error => console.error(error));
  }
  ngOnInit() {
    this.initForm();

    this.msgItem = new ChatMessageModel();
    this.msgForm.reset();
  }

  private subscribeToEvents(): void {

    this.chatRoomService.messageReceived.subscribe((message: ChatMessageModel) => {
      this._ngZone.run(() => {

        this.chatMessages.push(message);

        this.chatMessages = this.chatMessages.slice((-1) * this.chatLimit);
      });
    });
  }

  private initForm(): void {
    this.msgForm = new FormGroup({
      'message': new FormControl(null, [Validators.required])
    });
  }
  public sendMessage(): void {
    if (this.msgForm.invalid) {
      return;
    }

    this.msgItem.message = this.msgForm.value.message;

    this.http.post<ChatMessageModel>(this._baseUrl + 'api/chats/postmessage', this.msgItem).subscribe(result => {
      //this.chatMessages.push(result);
      this.msgForm.reset();
    }, error => console.error(error));
  }
}

