## \*\**Version 3.0.0 Currently in Development*\*\*
*What EZCode v3 is going to look like,*
```
make ^int {NAME} {VALUE} => var {NAME} new : {VALUE}
// turns: var name 0
// into: var name new : 0
// which is the valid way to create class instance

// color class that stores R, G, B values
// looks for c[R, G, B] and turns that into a new color instance
class color {
    explicit watch c\[{R}, {G}, {B}\] => set : R, G, B
    int R 0
    int G 0
    int B 0
    method set : @int:r, @int:g, @int:b {
        R = r
        G = g
        B = b
    }
}

// prints color with the new instance of color class c[50, 60, 90]
printColor : c[50, 60, 90]

method printColor : @color:c {
    print The color: 'c:r' 'c:g' 'c:b'
}
// Outputs:
// The color: 50 60 90 
```


![Main Image](https://raw.githubusercontent.com/JBrosDevelopment/EZCode/master/docs/Images/EZCode_Wide_Logo.png)
---

<details open>
<summary><h2>Welcome!</h2></summary>

**Welcome to EZCode!** EZCode is a comprehensive programming language built off of Microsoft WinForms. EZCode strives to make it as easy as possible to build a Windows program ranging from a Visual Application to simple console programs. Refer to [Community](#community) to learn more on how to contribute and where to join our Discord Server!
</details>

<details open>
<summary><h2>Docs</h2></summary>

The [Official Docs](https://github.com/EZCodeLanguage/EZCode/wiki/EZCode-Docs) are on the our [GitHub Wiki Page](https://github.com/EZCodeLanguage/EZCode/wiki). Please refer to this for any detailed instrictions.
</details>

<details open>
<summary><h2>Community</h2></summary>

Contribute to the community in many ways including the [EZCode Project Repository](https://github.com/EZCodeLanguage/EZCode-Projects.git). Create a pull request and it will be accepted as soon as possible. 

Please join our [Discord Server](https://discord.gg/DpBrp6Zy) to get closer to the community!
</details>

<details open>
<summary><h2>License</h2></summary>

EzCode is released under the [MIT License](LICENSE).
</details>
