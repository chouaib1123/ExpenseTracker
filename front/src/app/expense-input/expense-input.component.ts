import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BudgetService } from '../Services/budget.Service';

@Component({
  selector: 'app-expense-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './expense-input.component.html',
  styleUrls: ['./expense-input.component.css'],
})
export class ExpenseInputComponent {
  expenseAmount: number | null = null;
  expenseCategory: string = '';
  categories: string[] = [
    'Food',
    'Transport',
    'Utilities',
    'Entertainment',
    'Healthcare',
    'Education',
    'Shopping',
    'Travel',
    'Housing',
    'Miscellaneous',
  ];

  constructor(private http: HttpClient, private budgetService: BudgetService) {}

  onSubmit() {
    if (
      this.expenseAmount !== null &&
      this.expenseCategory &&
      this.expenseCategory !== ''
    ) {
      const now = new Date();
      const expenseData = {
        amount: this.expenseAmount,
        date: now.toISOString(),
        category: this.expenseCategory,
      };

      this.http
        .post<{ id: number; amount: number; date: string; category: string }>(
          'http://localhost:5251/api/Expense',
          expenseData,
          { withCredentials: true }
        )
        .subscribe({
          next: (response) => {
            console.log('Expense added successfully:', response);
            this.budgetService.addToCurrentAmount(this.expenseAmount!);
            this.budgetService.notifyExpenseAdded(response);
          },
          error: (err) => {
            console.error('Error adding expense:', err);
          },
        });
    }
  }
}
