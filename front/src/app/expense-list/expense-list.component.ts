import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { BudgetService } from '../Services/budget.Service';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css'],
})
export class ExpenseListComponent implements OnInit {
  expenses: { id: number; amount: number; category: string; date: string }[] =
    [];

  constructor(private http: HttpClient, private budgetService: BudgetService) {}

  ngOnInit() {
    this.fetchExpenses();

    this.budgetService.expenseAdded$.subscribe((expense) => {
      this.expenses.push(expense);
      console.log('Expense added:', expense);
    });
  }

  fetchExpenses() {
    this.http
      .get<{ id: number; amount: number; category: string; date: string }[]>(
        'http://localhost:5251/api/Expense',
        {
          withCredentials: true,
        }
      )
      .subscribe({
        next: (data) => {
          this.expenses = data;
          console.log('Expenses fetched:', this.expenses);
        },
        error: (err) => {
          console.error('Error fetching expenses:', err);
        },
      });
  }

  deleteExpense(id: number) {
    const expense = this.expenses.find((expense) => expense.id === id);
    if (expense) {
      this.http
        .delete(`http://localhost:5251/api/Expense/${id}`, {
          withCredentials: true,
        })
        .subscribe({
          next: (response) => {
            console.log('Expense deleted:', response);
            this.expenses = this.expenses.filter(
              (expense) => expense.id !== id
            );
            this.budgetService.subtractFromCurrentAmount(expense.amount);
          },
          error: (err) => {
            console.error('Error deleting expense:', err);
          },
        });
    }
  }
}
