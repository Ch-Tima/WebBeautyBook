import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {

  constructor(private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : ConfirmDialogData){
    this.dialog.backdropClick().subscribe(r => this.dialog.close(false))
  }

}

export class ConfirmDialogData{
  question: string|undefined
  warning: string|undefined
}
