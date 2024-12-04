import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BudgetService {
  private currentAmountSubject = new BehaviorSubject<number | null>(null);
  currentAmount$ = this.currentAmountSubject.asObservable();

  private expenseAddedSubject = new Subject<any>();
  expenseAdded$ = this.expenseAddedSubject.asObservable();

  setCurrentAmount(amount: number) {
    this.currentAmountSubject.next(amount);
  }

  subtractFromCurrentAmount(amount: number) {
    const currentAmount = this.currentAmountSubject.value;
    if (currentAmount !== null) {
      this.currentAmountSubject.next(currentAmount - amount);
    }
  }

  addToCurrentAmount(amount: number) {
    const currentAmount = this.currentAmountSubject.value;
    if (currentAmount !== null) {
      this.currentAmountSubject.next(currentAmount + amount);
    }
  }

  notifyExpenseAdded(expense: any) {
    this.expenseAddedSubject.next(expense);
  }
}
