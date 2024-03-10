export interface CompanyScheduleException {
    id: string;
    exceptionDate: string;
    openFrom: string;
    openUntil: string;
    isClosed: boolean;
    isOnce: boolean;
    reason: string;
    companyId: string;
  }