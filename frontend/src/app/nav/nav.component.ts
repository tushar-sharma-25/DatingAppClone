import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Observable } from 'rxjs';
import { User } from '_model/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model:any ={} // to store user entered data
  constructor(private accountService:AccountService) { }
  currentUser$ :Observable<User>;

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe(response =>{
      console.log(response);
    }, error => {
      console.log(error);
    })
  }

  logout(){
    this.accountService.logout();
    }

  

}
