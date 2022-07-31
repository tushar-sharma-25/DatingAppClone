import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  title = 'Dating App';
  users:any;
  url = 'https://localhost:7010/api/users';

  constructor(private http:HttpClient){}

  ngOnInit()  {
    this.getUsers();
  }

  getUsers(){
    this.http.get(this.url).subscribe({
      next:response => this.users=response,
      error:error => console.log(error)
    })
  }
}