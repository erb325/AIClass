/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mushrooms;

import java.util.Scanner;

/**
 *
 * @author erb39405
 */
public class Mushrooms {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in); 
        
        
        int code;
        char odor;
        char classification; 
        char gillSize;
        char gillColor;
        char sporePrintColor;
        char stalkSurfaceBelowRing; 
        char stalkSurfaceAboveRing;
        char habitat;
        char capColor;
        char stalkColorAboveRing;

        System.out.println("Welcome to the Mushroom Database");
        code();
        
        while (code()== 1){
        
            
        
            System.out.println("Please enter the ODOR key > ");
            odor = in.next().charAt(0);
        
            if (odor == 'f'){
                poisonous();
                continue;
            } 
        
            System.out.println("Please enter the GILL SIZE key > ");
            gillSize = in.next().charAt(0);
            System.out.println("Please enter the GILL COLOR key > ");
            gillColor = in.next().charAt(0);

            if (gillSize == 'n' && gillColor == 'b'){
                poisonous();
                continue;
            } else if (gillSize == 'n' && odor == 'p'){
                poisonous();
                continue;
            } else if (odor == 'c'){
                poisonous();
                continue;
            } 
            
            System.out.println("Please enger the SPORE PRINT COLOR key > ");
            sporePrintColor = in.next().charAt(0);
            
            if(sporePrintColor == 'r'){
                poisonous();
                continue;
            }
            
            System.out.println("Please enter the STALK SURFACE BELOW RING key > ");
            stalkSurfaceBelowRing = in.next().charAt(0);
            System.out.println("Please enter the STALK SURFACE ABOVE RING key > ");
            stalkSurfaceAboveRing = in.next().charAt(0);
            
            if(stalkSurfaceBelowRing == 'y' && stalkSurfaceAboveRing == 'k'){
                poisonous();
                continue;
            }
            
            System.out.println("Please enter the HABITAT key > ");
            habitat = in.next().charAt(0);
            System.out.println("Please enter the CAP COLOR key > ");
            capColor = in.next().charAt(0);
            if(habitat == 'l' && capColor== 'w'){
                poisonous();
                continue;
            }
            System.out.println("Please enter the STALK COLOR ABOVE RING key > ");
            stalkColorAboveRing = in.next().charAt(0);
            if (stalkColorAboveRing == 'y'){
                poisonous();
                continue;
            }
            
            edible();
            
             
        }
    }
    public static void poisonous (){
        System.out.println("Your mushroom is poisonous");
    }
    public static void edible(){
        System.out.println("Your mushroom is edible");
    }
    public static int code(){
        int code;
        System.out.println(" 1- Classify a mushroom 0- to exit > ");
        code = in.nextInt(); 
        return code; 
    }
}
