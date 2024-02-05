export class NavMenuItem{
    
    iconPath: string
    title: string
    value: string
    sublist: NavMenuItem[] = []

    constructor(iconPath: string, title: string, value: string){
      this.iconPath = iconPath;
      this.title = title;
      this.value = value;
    }
  }
  