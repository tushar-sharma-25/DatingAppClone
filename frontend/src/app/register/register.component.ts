import { AccountService } from './../_services/account.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model:any={};
  
  @Output() cancelledRegister = new EventEmitter();

  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(response => {
      this.cancel();
    }, error => {
      console.log(error);
    })
      
  }

  cancel(){
    this.cancelledRegister.emit(false);
  }

}
