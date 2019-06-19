import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ItemsService } from 'src/app/services/items.service';
import { Message } from 'src/model';
import { OrderService } from 'src/app/services/order.service';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styles: []
})
export class MessagesComponent implements OnInit {
  @Output() setMessagesCount = new EventEmitter();
  @Output() closeMessages = new EventEmitter();
  messages: Message[] = [];
  orderDetailsOpened = false;
  order = null;
  constructor(
    private orderService: OrderService,
    private messagesService: MessagesService
  ) {}

  async ngOnInit() {
    this.messages = await this.messagesService.getMessages();
    this.setMessagesCount.emit(this.messages.length);
  }

  async showDetails(msg: Message) {
    this.order = await this.orderService.get(msg.orderId);
    await this.messagesService.setRead(msg.id);
    this.orderDetailsOpened = true;
    this.messages = this.messages.filter(m=>m.id !== msg.id);
  }

  close() {
    this.closeMessages.emit();
  }

  closeDetails() {
    this.order = null;
  }
}
