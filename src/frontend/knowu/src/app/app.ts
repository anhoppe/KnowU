import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private http = inject(HttpClient);
  protected readonly title = signal('knowu');

  noteText = '';
  
  submit() {
    if (!this.noteText.trim()) {
      console.log('Note is empty');
      return;
    }

    this.http.put('http://localhost:5260/notes', `"${this.noteText}"`, {
      headers: { 'Content-Type': 'application/json' }
    }).subscribe({
      next: (response) => {
        console.log('Note added successfully:', response);
        this.noteText = ''; // Clear the input after successful submission
      },
      error: (error) => {
        console.error('Error adding note:', error);
      }
    });
  }
}
