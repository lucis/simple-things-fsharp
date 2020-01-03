open System
open System.IO
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open System.Runtime.Serialization

let rec qsort: int list -> int list= function
    | [] -> []
    | x::xs -> let smaller = [for a in xs do if a<=x then yield a]
               let larger =  [for b in xs do if b>x then yield b]
               qsort smaller @ [x] @ qsort larger

let readFile(path: string) = File.ReadAllText path

[<DataContract>]
type Input =
   { 
      [<field: DataMember(Name = "list")>]
      list: int seq;
   }
 
[<DataContract>]
type Output =
   { 
      [<field: DataMember(Name = "result")>]
      result: int seq;
   }

let sortHandler: Input -> Output =
    fun input -> { result= qsort (Seq.toList input.list) }

let app = 
  choose
    [ POST >=> choose
        [path "/" >=> Json.mapJson sortHandler]]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig app
    0
