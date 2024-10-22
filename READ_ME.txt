/*************************************************************************
 * SIG-Games Workshop
 * 
 * Main Topics: Finite State Machines
 * Sub Topics: Singleton Design Pattern
 *             State Design Pattern
 * 
 * Purpose: The main goal of this workshop is to introduce finite state 
 *          machines (FSM) and demonstrated how they can be applied in 
 *          the context of games. In the case of this example we use
 *          an FSM to control our goblin lumberjack as he goes on the 
 *          world and attempts to gather wood.
 
 *          As an alternate goal this workshops attempts to introduce
 *          some design patterns along with their implementation and
 *          ussage. 
 *          State Design Pattern: Used in order to create or FSM.
 *          Singlton Design Patter: Used to maintain only one
 *                                  one instance of each state at
 *                                  any point in time. Since all the 
 *                                  state information is held by the 
 *                                  owner of the state there is no
 *                                  need to create new instance of the 
 *                                  states every time.
 *                                  
 * Author: Frank E. Hernandez
 * A.K.A.: CodeMinion
 * Site: http://www.cs.fiu.edu/~fhern006
 * 
 
 * Art:
 * 
 * Author: Yar
 * Filename: iso-64x64-outside
 * Link: http://opengameart.org/content/isometric-64x64-outside-tileset
 * 
 * Author: Blarumyrran
 * Filename: human-city
 * Link: http://opengameart.org/content/old-stone-buildings
 * 
 * 
 * Author: Clint Bellanger
 * Filename: goblin_lumberjack_black.png
 * Filename: goblin_lumberjack_blue.png
 * Filename: goblin_lumberjack_cyan.png
 * Filename: goblin_lumberjack_green.png
 * Filename: goblin_lumberjack_orange.png
 * Filename: goblin_lumberjack_pink.png
 * Filename: goblin_lumberjack_purple.png
 * Filename: goblin_lumberjack_red.png
 * Filename: goblin_lumberjack_white.png
 * Filename: goblin_lumberjack_yellow.png
 * Link: http://opengameart.org/content/goblin-lumberjack 
 * 
 * Author: qubodup
 * Filename: pointer
 * Link: http://opengameart.org/content/bw-ornamental-cursor-19x19
 * 
 * 
 * Audio:
 * Author: qubodup
 * Filename: click-click-mono
 * Link: http://opengameart.org/content/click-ui-menu-sfx-yesnoselect
 * 
 * 
 * ***********************************************************************/
