import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BudgetService } from '../Services/budget.Service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-budget',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './budget.component.html',
  styleUrls: ['./budget.component.css'],
})
export class BudgetComponent implements OnInit {
  CurrentAmount: number | null = null;
  Budget: number | null = null;
  newBudget: number | null = null;
  message: string = '';

  constructor(private http: HttpClient, private budgetService: BudgetService) {}

  ngOnInit() {
    this.fetchBudget();

    this.budgetService.currentAmount$.subscribe((amount) => {
      if (amount !== null && this.Budget !== null) {
        this.CurrentAmount = amount;
        if (this.Budget < amount) {
          this.message = 'Budget exceeded';
        } else {
          this.message = '';
        }
      }
    });
  }

  fetchBudget(){
    this.http
    .get<{ id: number; currentAmount: number; budget: number }>(
      'http://localhost:5251/api/Budget',
      {
        withCredentials: true,
      }
    )
    .subscribe({
      next: (data) => {
        console.log('Budget data:', data);
        this.CurrentAmount = data.currentAmount;
        this.Budget = data.budget;
        this.budgetService.setCurrentAmount(this.CurrentAmount);
        console.log(
          'Current amount:',
          this.CurrentAmount,
          'Max amount:',
          this.Budget
        );
      },
      error: (err) => {
        console.error('Error fetching budget data:', err);
      },
    });
  }

  onSubmit() {
    if (this.newBudget !== null && this.newBudget > 0) {
      this.http
        .post<{ message: string; oldMax: number; newMax: number }>(
          'http://localhost:5251/api/Budget',
          this.newBudget,
          {
            withCredentials: true,
          }
        )
        .subscribe({
          next: (response) => {
            console.log('Budget updated:', response);
            this.Budget = response.newMax;
            this.newBudget = null;
            if (this.Budget < this.CurrentAmount!) {
              this.message = 'Budget exceeded';
            } else {
              this.message = '';
            }
          },
          error: (err) => {
            console.error('Error updating budget:', err);
          },
        });
    }
  }
}
