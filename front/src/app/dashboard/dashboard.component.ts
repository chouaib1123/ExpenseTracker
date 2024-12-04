import { Component } from '@angular/core';
import { BudgetComponent } from '../budget/budget.component';
import { ExpenseListComponent } from '../expense-list/expense-list.component';
import { ExpenseInputComponent } from '../expense-input/expense-input.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    BudgetComponent,
    ExpenseListComponent,
    ExpenseInputComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {}
