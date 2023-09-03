import { Assignment } from "./Assignment"
import { BaseUser } from "./BaseUser"
import { Comment } from './Comment'

export class Appointment{
    id: string|undefined 
    forWhatTime: string|undefined
    note: string|undefined
    status: string|undefined 
    userId: string|undefined 
    baseUser: BaseUser|undefined 
    assignmentId: string|undefined
    assignment: Assignment|undefined
    commentId: string|undefined
    comment: Comment|undefined
}