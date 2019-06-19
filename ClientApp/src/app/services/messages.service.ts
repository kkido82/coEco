import { Injectable } from '@angular/core';
import { apiGet, apiPost } from 'src/app/helpers/api';
import { Message } from 'src/model';

const MESSAGES_URL = '/api/messages/';
const MESSAGE_READ = MESSAGES_URL + 'read/';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  getMessages() {
    return apiGet<Message[]>(MESSAGES_URL);
  }

  setRead(id:number) {
    return apiPost(MESSAGE_READ + id, {});
  }
}
