namespace AdrCli.Tests
open Hedgehog
open Hedgehog.Gen
open AdrCli.Configuration
open AdrCli.Optional

module SettingsTests =    
    let settings : Gen<Settings> =
        gen {
            let! template = Gen.string (Range.constant 1 100) Gen.alphaNum
            let! repositoryPath = Gen.string (Range.constant 1 100) Gen.alphaNum
            let! editor = Gen.string (Range.constant 1 100) Gen.alphaNum
            return { 
                repositoryPath = repositoryPath
                template = template
                editor = editor 
            }
        }
        
    let deserialiseIsLeftInverseForSerialise =
        Property.forAll
            settings
            (fun s -> (deserialiseSettings << serialiseSettings) s = optional.Return s |> Property.ofBool)