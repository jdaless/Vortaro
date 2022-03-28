# Universala Vortaro Demokratia

Reta vortaro, tekstaro, kaj tradukilo. La vortaro enhavas aĵojn kaj oficialajn (de la Akademio, ktp) kaj kreitajn de uzantoj. 

## Eki
La UVD estas apo farita de .NET6. Post instalado de [dotnet](https://dotnet.microsoft.com/en-us/download), oni devos krei la komencan datumbazon. La projekto `Servilo` ankaŭ estas ilo por krei tian datumbazon el dosiero `.csv`. Ekzemple, 
``` bash
dotnet run  --project ./src/Server/ dosieroj/fundamentaVortaro.csv
```
Tio uzas la klason `Krei`, kiu traktas la csv'on kaj kreas la datumbazon. 
Poste, oni simple povas uzi `dotnet run --project .\src\Server\` por komencigi la servilon. Iru al `https://localhost:7095` por uzi la apon. 

La apo ankaŭ uzas Auth0 por aŭtentigo, do vi bezonos ŝlosilojn de tio por ke la apo funkcias. 

## Kiel ĝi funkcias


## Gastigado
Mi ankoraŭ provas sciiĝi la plej bona rimedo por gastigi la servilon. La apo senutilas se vi ne estas sur la TTT. Mi jam aĉetis la adreson `uvdemokratia.com`. 

## Helpu min
Malfermu "Issue"ojn kaj faru "Pull Request"ojn. Mi volas la helpon de iu ajn kiu volas doni ĝin! Mi bezonas helpon ĉefe pri la jenaj fakoj:

* Esperantigi la kodon (anstataŭi la "boilerplate"ajn anglajn vortojn)
* Plibonigi la aspekton de la apo (per CSS ktp)
* Rapidigi la apon

## Aliaj Notoj
`fundamentaVortaro.csv` venas rekte de la Akademia Vortaro. Mi faris etajn ŝanĝojn por ke ĝi funkciu kun la apo. Duoblajn vortojn kaj elementojn kun pli ol unu vorto mi forigis. Ideale, ĝi enhavus la tutan akademian vortaron kaj eble aliajn fidindajn fontojn (PIV, la tradukoj el Tujavortaro, ktp) sed tio ne estas facila tasko. 

Kiam mi komencigos la retejon, mi plenigos la datumbazon per la oficialajn fontojn kaj poste ĉio venos de uzantoj. 

