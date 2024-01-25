Shader "Custom/CellShading1" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowColor1 ("Shadow Color 1", Color) = (0,0,1,1)
        _ShadowColor2 ("Shadow Color 2", Color) = (0,0,0,1)
        // Ajoutez d'autres propriétés ici au besoin
    }
    SubShader {
        // Tags, Pass, etc.

        CGPROGRAM
        // Déclarations de pragma et d'inclusion

        // Déclaration de variables uniformes pour les couleurs d'ombre
        uniform float4 _ShadowColor1;
        uniform float4 _ShadowColor2;

        // Fonction fragment pour le cell shading
        fixed4 Frag (v2f i) : SV_Target {
            // Logique pour déterminer le niveau d'ombre et sélectionner la couleur
            // Cette partie dépend de votre algorithme spécifique de cell shading

            return fixed4(0,0,0,1); // Retourne une couleur d'ombre en fonction de la logique ci-dessus
        }
        ENDCG
    // Ajoutez d'autres SubShaders et Passes au besoin
}