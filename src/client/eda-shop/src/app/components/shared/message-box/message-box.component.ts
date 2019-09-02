import { Component, OnInit, Inject } from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

export interface MessageBoxData {
  title: string;
  text: string;
}

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css']
})
export class MessageBoxComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<MessageBoxComponent>,
              @Inject(MAT_DIALOG_DATA) public data: MessageBoxData) { }

  ngOnInit() {
  }

  onOkClick(): void {
    this.dialogRef.close();
  }
}
