import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'filter' })
export class FilterPipe implements PipeTransform {
  transform(items: any[], searchText: string, propertyName: string): any[] {
    if (!items) {
      return [];
    }
    if (!searchText) {
      return items;
    }
    searchText = searchText.toLocaleLowerCase();

    return items.filter(it => {
      if(propertyName.length == 0){
        return it.toString().toLocaleLowerCase().includes(searchText);
      }else{
        return it[propertyName].toString().toLocaleLowerCase().includes(searchText);
      } 
    });   
  }
  
}
