import { PipeTransform, Pipe } from '@angular/core';

@Pipe({ name: 'statunsName' })
export class StatusNamePipe implements PipeTransform {
  transform(value: number): any {
    switch (value) {
      case 0:
        return 'הבקשה נדחתה';
        // break;
      case 1:
        return 'הבקשה אושרה';
        // break;
      case 2:
        return 'הבקשה ממתינה לאישור';
        // break;
      case 3:
        return 'הבקשה אושרה וממתינה להעברה';
        // break;
      case 4:
        return 'הבקשה הסתיימה';
        // break;
    }
  }
}
