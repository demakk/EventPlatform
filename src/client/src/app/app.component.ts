import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title: string = 'client';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:44316/api/Identity').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request was completed'),
    });
  }
}
