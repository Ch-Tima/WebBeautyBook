import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'filter' })
export class FilterPipe implements PipeTransform {
  // Custom Angular pipe to filter an array of items based on a search text and a property name
  transform(items: any[], searchText: string, propertyName: string): any[] {
    if (!items) {
      return [];
    }
    if (!searchText) {
      return items;// If there's no searchText provided, return all items as is
    }
    // Convert the searchText to lowercase for case-insensitive comparison
    searchText = searchText.toLocaleLowerCase();
    // Filter the items based on the searchText and optionally propertyName
    return items.filter(it => {
      if(propertyName.length == 0){// If no propertyName is provided, convert the item to a string and check for a match
        return it.toString().toLocaleLowerCase().includes(searchText);
      }else{// If a propertyName is provided, access the property value and check for a match
        return it[propertyName].toString().toLocaleLowerCase().includes(searchText);
      }
    });
  }

}
