﻿module PodVocasem.Client.Pages.Index

open Feliz
open Elmish
open Feliz.UseElmish
open PodVocasem.Client
open SharedView

type State = {
    Message : string
}

type Msg =
    | AskForMessage
    | MessageReceived of string

let init () = { Message = "Click me!" }, Cmd.none

let update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | AskForMessage -> model, Cmd.OfAsync.perform Server.service.GetMessage () MessageReceived
    | MessageReceived msg -> { model with Message = msg }, Cmd.none

let logo =
    Svg.svg [
        svg.className "w-24 sm:w-32 md:w-40"
        svg.viewBox (0, 0, 1000, 1620)
        svg.fill "none"
        svg.children [
            Svg.path [
                svg.d "M913.5 1084.9C912.1 1078.7 910.6 1072.5 907.8 1066.7C901.3 1057.5 894.2 1048.1 888.5 1038.2C882.6 1027.2 876.4 1016.4 871.5 1004.9C869.2 998.6 866.2 992.5 863.8 986.2C861 977.8 858 969.4 855.1 961C852 951 847.1 940.9 837.6 935.7C829.6 931.6 822.4 926.1 815.7 920.1C806.4 912.6 799.9 902 797.4 890.3C795.1 876.5 792.5 862.4 794.8 848.4C796.5 838.8 798.5 829.7 802 820.5C805.3 812.6 809.4 805.1 813 797.4C815.5 791.6 818.9 785.2 820.7 779C821.4 776.6 822.2 774.1 822.8 771.6C823.4 771.7 824 771.6 824.7 771.5H824.8C825.1 773 825.5 774.4 825.6 775C827.5 783.8 835.7 789 841.2 795.6C845.4 800.3 849.3 805.3 852.5 810.7C857.9 822.4 855.9 836.9 865 847.1C869.1 852.8 875.4 855.7 881.8 858C887.4 860.1 892.9 862.6 898.9 863.6C901.5 864 904.2 863.9 906.7 864.6C908.4 865.1 910.4 863.6 910.3 861.8C910.2 860.7 909.7 860 909 859.2C906.9 857.1 905 854.8 903.2 852.4C900.3 848.5 899.4 843.7 898.2 839.1C897 834.3 895.7 829.5 894.8 824.6C892.7 812.7 891.2 800.7 888.6 788.9C886.6 780.3 883.8 771.9 881.1 763.5C875.4 743 867.3 724 853.1 707.7C848.3 700.4 842.3 694.2 835.8 688.4C825.1 677.7 811.3 669.9 795.9 669.4C791.7 669.3 787.5 670 783.3 670.7C780.7 670.9 777.8 672 776.7 674.1C776.5 674.1 776.3 674.3 776.4 674.6C773 670.8 760.7 656.5 756 657.3C755.6 657.4 755.3 657.6 755 657.8C755.7 651.7 753.1 644.1 749.5 639.8C748.2 638.4 746.3 638.1 744.5 638.1C741.6 637.9 738.7 637.1 735.9 636.3C722.9 632.6 710.2 626.9 696.8 625C686.4 623.2 675.8 623.2 665.5 620.6C664.2 620.4 662.6 620 661.2 619.8C661.2 619.8 661.2 619.8 661.1 619.8C660.2 617 658.8 614.3 657.5 611.6C652.5 601.2 646.3 591.4 640.5 581.4C635.4 572.8 630.4 564.4 625.8 555.5C621.1 547.3 615.7 539.4 611.3 531C606.1 520 599 509.5 593.5 498.6C590.1 491.3 588.3 483.3 587.5 475.3C587 466.4 586.4 457.3 585.3 448.3C584.3 441.7 584 434.7 581 428.6C577.1 419.6 568.2 414.5 559.4 411.2C555.7 410.1 551.8 409.4 548.3 407.8C545.7 406.5 542.9 405.1 540.7 402.9C539.3 398.2 540.4 393.1 540.2 388.2C540.2 378 538.6 368 537.1 357.9C535.1 345.2 533.9 330.9 524.7 321.1C520.5 316.1 514.8 312.8 508.3 311.9C505 311.3 501.6 310.5 498.3 311.4C491.7 313.8 485.9 317.7 480.9 322.7C477 325.9 472.8 328.8 469.4 332.6C463.5 338.7 458.4 345.7 455.2 353.7C450.7 364.5 445.5 375.3 444 387C443.8 388.5 443.7 390 443.5 391.5C443.1 394 446.4 395.8 448.3 394.1C448.9 393.6 449.3 392.9 449.4 392.2C449.6 388.3 450.3 384.5 451.3 380.8C452 379.8 452.8 379 453.7 378.2C455.1 377.4 456.5 376.2 458.1 376.3C460.4 377.7 461 381.1 460 383.4C457.7 387.7 453.3 390.9 452.2 395.9C451.2 399.5 451.3 404.1 455.2 405.8C457.7 407.4 461 407.2 463.4 409.1C471.8 416.7 465.5 425.8 471.4 429.1C472.8 429.9 474.2 429.6 475.2 428.7C475.3 428.7 475.4 428.8 475.6 428.8C477.9 429.2 480.2 428.9 482.7 429.5C483.7 431.4 482.8 433.5 481.9 435.3C480.1 438.7 477.7 442.8 475.9 446.4C471.3 456.2 467.3 466.4 462.4 476.1C459.6 481.9 455.7 487.1 452.3 492.6C451.9 493.1 451.5 493.5 451.1 493.9C450.5 493.9 449.9 493.9 449.5 493.9C446 492.6 443 490.1 439.9 488C432.4 482.7 425.9 476.2 419.2 470.1C412.7 464.8 406.8 458.7 402.8 451.2C399.1 441.5 396.8 431.9 392.2 422.3C389.7 416.8 386 411.9 382.1 407.3C380.1 404.8 376.6 400.3 373.5 403.9C373.4 404.1 373.3 404.2 373.2 404.4C372.9 403.1 372.5 401.8 372.1 400.5C362.7 360.1 348.8 321 337.5 281.2C334.9 270.4 332.2 259.7 329.5 249C329.1 246.9 328.6 244.9 328.1 242.8C330 242.9 331.9 243.1 333.8 243.4C345.9 246 357.6 250.1 369.4 253.8C387.2 260.1 406.6 269.3 413.9 288C414.9 290.6 415.6 293.4 416.5 296.1C416.8 297.3 417.5 298.5 418.8 298.6C420.9 298.8 421.4 296.5 421.1 294.9C420.7 289.7 419.8 284.6 418.4 279.6C415 268.1 411.1 256.5 403.7 246.9C401.1 242.9 398.4 239.2 395.1 235.9C389.2 229.7 383.1 222.7 376 217.6C369.7 213.1 363.5 208.3 357 204.2C352.7 201.4 347.8 199.6 343.3 197.3C336.6 193.3 329.5 190 322.8 186C319.1 183.6 315.9 180.5 312.3 177.9C310.9 177 309.5 176 308.3 174.9C307 170.8 305.5 166.7 304.2 162.5C302.4 155.8 300.4 149.1 298.5 142.4C295.8 132.3 291.9 122.6 289.4 112.5C287.9 103.9 284.4 95.9999 282.1 87.6999C281.1 82.3999 278.7 77.5999 276.7 72.6999C276.3 71.0999 275.4 69.2999 273.7 69.8999C273.3 68.8999 272.9 67.9999 272.2 67.2999C271.8 66.8999 271 66.6999 270.5 67.0999C269.7 67.5999 269.7 68.7999 270.2 69.5999C271.6 78.4999 275 86.8999 276.8 95.6999C281.6 116.1 288.1 136 293.5 156.2C294.7 161.7 296.3 167 297.9 172.4C297.8 178.9 299.1 185.5 299.5 192.1C300.6 203.4 302.3 214.8 305.2 225.9C307 231.4 307.1 238.1 311.9 242C314.2 243.9 317 244.1 319.9 243.9C320.3 245.3 320.7 246.8 321.2 248.2C324.7 262.1 328.5 275.9 332.1 289.8C335.1 301.6 337.2 313.2 340.6 325C344 334.8 346.3 345 349.3 354.9C355.8 375.1 361.9 395.4 367.8 415.7C367.5 415.5 367.1 415.4 366.7 415.4C361.8 415.3 363.2 421.6 362.9 424.9C362.8 430.5 364.1 435.9 365.5 441.3C366.6 446.3 368.6 451 372 454.9C374.5 458.3 376 461.5 378.1 465.3C381.5 471.2 385.3 476.3 388.2 482.5C392.5 490.8 397 498.6 399.6 507.8C401.2 513.6 404.2 518.8 406.8 524.2C409.6 528.8 412.6 533.4 413.8 538.8C415.7 547.7 416 556.8 409.3 563.9C407.2 566 404.1 566.5 401.2 566.9C400.4 567 399.7 567.1 399 567.2C398.6 567.2 398.1 567.3 397.8 567.5C397.3 566.9 396.8 566.3 396.3 565.8C392.9 562.7 390.2 558.9 386.4 556.2C382.2 553.4 378.7 549.9 374.9 546.6C368 541.4 361 536.4 353.8 531.6C343.9 525.4 333.2 520.8 322.5 516.4C313.2 511.2 303.3 507.3 293.2 504C285 501.2 276.4 500.8 267.9 499.7C257 498.5 246.2 499.6 235.3 500.7C216.5 502.2 197.8 505.7 180 512C172.2 514.6 164.3 517.4 157.5 522.2C156.4 522.8 155.3 523.5 154.2 524.1C149.8 519.8 145.8 515 140.2 512.2C136 509.9 131.5 508.3 127.5 505.8C125.8 504.8 124.3 503.4 122.4 502.8C117.9 501.6 114.3 506.7 116.4 510.7C119.1 517.2 120.6 524.1 122.5 530.9C123.9 536.3 126.9 543 123.3 548C118 547.5 112.3 548.9 107.4 546.2C104.9 545.1 102.8 543 100.1 542.6C95.5 542 92.8 547.5 95.2 551.1C98 555.2 101.6 558.7 104.8 562.5C114.1 571.2 114.3 571.9 116 584.5C118.4 598.8 122 613.6 132.3 624.4C137 630.1 141.5 635.9 145.8 641.9C150.4 649.1 154.9 656.3 160.5 662.7C164.8 668.5 169.6 673.8 174.8 678.8C179.3 683.7 183.4 689 187.3 694.3C192 700.5 194.3 707.9 198.2 714.5C204.5 723.8 212.4 734.7 224.4 736.5C234.8 737.7 240.7 731.1 247.9 725C260.2 717.7 263.9 706.4 264 694C273.3 688.4 282.6 682.7 290.8 675.7C291 676.2 291.2 676.7 291.4 677.1C291.5 677.9 291.7 678.7 292.2 679.5C292.4 679.8 292.7 680.1 292.9 680.4C294.3 683.5 295.8 686.5 297 689.6C299.4 697.5 302.8 705.2 303 713.7C304.1 721.6 303.8 729.8 302.4 737.8C301.6 743.3 302.2 748.8 302.2 754.3C302.1 760.1 303.1 765.9 305 771.5C307.6 784.4 314.1 797 324.3 805.5C325.9 806.7 327.5 807.9 329.1 809.1C332.7 813.1 336.5 816.8 340.1 820.1C339.9 822.3 339 824.4 338.3 826.5C337 828.6 335.4 829.9 333.6 831.7C325.9 839.5 316 844.4 306.5 849.6C295.7 855.6 283.7 859.4 272.5 864.6C265.9 867.2 259.3 870.3 254 875.1C247.6 881.9 243.1 891 241.7 900.3C241.2 906.3 242.4 912.6 244.2 918.3C246.8 922.9 248.3 928 249.8 933C251.2 937.3 253.2 941.5 254.6 945.8C259.3 957.2 261.7 969.6 263.5 981.8C264.9 989.9 263.8 998.5 267.5 1006.1C268.8 1009.6 271.3 1012.3 274 1014.8C275.1 1016.1 276 1017.6 277.5 1018.4C279.2 1019.4 281.2 1019.3 282.8 1020.4C285.4 1022.7 287.9 1024.8 290.4 1027.3C300.5 1040.3 309.9 1054.9 324.4 1063.5C328.1 1066 331.8 1069.2 336.3 1070.4C338.6 1071.4 341.2 1072 343.7 1071.3C348 1071.4 349.1 1066.8 350.4 1063.5C353.6 1053.7 353.9 1042.9 351.5 1032.8C350.3 1025.1 350.9 1017.7 344 1012.5C341.8 1010.7 339.4 1009.3 337.6 1007.1C330.9 999.9 320.6 1001.9 312 1000.2C309.7 999.4 309.9 996.6 309.2 994.6C307.7 991.3 307.9 987.7 306.5 984.4C305.8 982.9 304.9 981.5 304 980.1C302.1 977.3 299.7 975 298.1 971.8C295.8 965.7 295.4 959.1 293.9 952.8C292.4 946.1 290 939.6 288.3 933C285.9 925 285.9 924.1 289.9 916.7C291.2 914.5 291.6 911.4 293.8 909.9C299.3 908.1 305.2 907.4 310.7 905.6C326.2 900.2 342.6 899.2 358.4 894.8C362.2 894 366.1 893.4 370 892.8C378.4 891.5 386.8 890.2 394.7 887.1C399 885.1 402.3 881.3 405 877.4C407.4 873.9 410 870.4 411.9 866.5C412.5 864.2 415.7 862.9 415.3 860.4C417.7 860.4 420.1 860.2 422.5 860.1C426.6 860 434.2 857.7 436 862.7C436.5 867 434 871.2 432.5 875.2C429.9 881 425.3 885.8 419.6 888.5C413.5 891.6 407.5 895.1 401.3 898C395.7 900.4 390 902.9 386 907.6C376.7 918.2 387.2 922.1 395.4 927.1C400.7 930.6 407.5 929.4 413.6 929.4C427.8 929.3 441.8 926.2 456 926.2C463.6 925.8 471 923.6 477.2 919.1C477.2 921 477.3 922.7 477.4 923.4C477.7 927.8 477.5 932.2 478.5 936.5C480 943 483 949.1 485.8 955.2C489.2 962 494.6 967.5 499 973.7C503 979.4 504.2 986.4 505.7 993C509.2 1008.2 511.1 1023.7 511.1 1039.4C510.9 1053.8 515.5 1055.2 503.7 1066.9C497.6 1071.1 494.1 1077.8 491 1084.4C490.9 1084.7 490.8 1085 490.7 1085.2H70.2V1144.9H939.9V1085.2H913.5V1084.9ZM352.5 347.6C353.2 349.7 353.9 351.7 354.7 353.8C358.2 362.9 360.8 372.2 363.1 381.7C359.6 370.3 356 359 352.5 347.6ZM260.3 666.9C259.4 662.1 258.5 657.4 257.8 653C257.1 647.5 256.4 642 255.9 636.5C255.8 634.5 255.4 632.4 256.5 630.6C262.4 632.5 267.3 636.4 271.5 640.9C273.6 643 275.8 645.1 277.8 647.3C278.5 649.1 279.4 650.9 280.3 652.6C278.3 654.2 276.3 655.9 274.2 657.5C269.8 660.6 265.5 663.7 260.9 666.6C260.7 666.6 260.5 666.7 260.3 666.9ZM268.8 682.3C266.9 683.5 265 684.7 263.2 685.9C262.9 681.9 262.3 678 261.6 674C268.4 670.7 274.5 666 280 660.7C281.3 659.8 282.5 658.9 283.8 658C284.1 658.4 284.3 658.8 284.6 659.2C285.7 661.5 286.8 663.7 287.8 666C287.9 666.6 288 667.3 288.1 667.9C281.9 673.1 275.6 678.1 268.8 682.3ZM550.5 1083.5C549.1 1079.5 547 1075.6 546.5 1071.3C546.3 1068.3 548.3 1065.8 549.5 1063.2C550.8 1060.4 552.1 1057.4 552.4 1054.3C553 1045.6 550 1037.3 546.5 1029.5C543.2 1020.3 542.5 1010.4 540 1000.8C536.2 984.4 532.7 967.8 531 951.1C531.1 949.2 531.3 947.3 532 945.5C533 943.4 534.5 941.8 535.6 939.7C537.4 936.4 537.7 932.6 537.5 929C536.6 917.7 535.6 906.5 532.9 895.5C530.9 885.6 525.9 876.2 526.4 865.9C531 867.4 535.3 870.1 540.2 870.5C554.6 872.4 569 870.5 583.4 869.1C594.8 867.5 605.3 862.7 615.7 858C623.8 854.9 631.5 851.2 639.2 847.4C644 845.1 648.2 842 652.8 839.5C653.1 843.3 653.8 847.1 655.2 850.7C657 855.5 661.5 859.5 665.2 863.1C670.1 867.8 675.4 872.2 681.7 874.9C687.2 877.7 692.1 881.4 697.3 884.7C707 891.3 716.9 897.3 725.4 905.5C735 915.1 744.4 925.9 748.6 939.1C748.7 939.4 748.8 939.7 749.1 939.9C743.3 947.5 734.8 952.3 728.2 959.1C720.5 968.4 706.5 974.1 705.7 987.6C704.4 994.8 706.2 1001.9 707.3 1009.1C708.7 1017.8 710.2 1026.5 711.3 1035.3C712.5 1044.1 713.9 1052.8 715.7 1061.5C717 1065.8 716.5 1074.1 720.8 1076.5C724.5 1077.6 727.4 1073.9 729.8 1071.6C732.5 1068.9 735.4 1066.3 738.4 1063.9C741.9 1061.2 745.2 1058.4 748.5 1055.5C755.1 1050.1 763.5 1043.4 761.9 1033.8C761 1028.4 756.5 1024.4 754.8 1019.3C753.8 1016.4 752.6 1013.4 752.7 1010.3C753.6 1004.9 755.9 998.7 752.4 993.7C758.3 990.3 763.4 986 767.8 980.9C771.1 977.9 774.2 974.7 776.9 971.1C778.2 969.5 779.6 968 781.1 966.6C781.6 965.9 781.8 965.1 781.7 964.3C783.8 966 785.5 968.1 787.7 969.7C791.7 973.3 796.4 976.2 799.7 980.5C801.4 983 802.3 986 803.9 988.6C807.4 994 812 998.7 817.1 1002.6C821 1005.4 825.2 1006.8 829 1009.9C835.7 1016.9 840.5 1025.7 845.5 1034.1C849.9 1042.6 853.9 1051.4 856.9 1060.5C858 1063.9 859 1067.3 859.9 1070.7C860.8 1075.4 861.6 1080.1 862.3 1084.9H550.8C550.8 1084.4 550.6 1084 550.5 1083.5Z"
                svg.fill "black"
            ]
            Svg.path [ svg.d "M77.5 1205.8C77.5 1204 79 1202.3 81 1202.3H122.9C145.5 1202.3 164.1 1220.7 164.1 1242.9C164.1 1265.7 145.5 1284.2 123.1 1284.2H96.5V1327.4C96.5 1329.2 94.8 1330.9 93 1330.9H81C79 1330.9 77.5 1329.2 77.5 1327.4V1205.8ZM121.8 1266.2C134.5 1266.2 145.1 1255.9 145.1 1242.7C145.1 1230.2 134.4 1220.6 121.8 1220.6H96.4V1266.2H121.8Z"; svg.fill "black" ]
            Svg.path [ svg.d "M243.3 1200.4C280.1 1200.4 309.5 1230 309.5 1266.7C309.5 1303.4 280.1 1332.7 243.3 1332.7C206.5 1332.7 177.3 1303.5 177.3 1266.7C177.3 1229.9 206.5 1200.4 243.3 1200.4ZM243.3 1314.4C269.6 1314.4 291.1 1293.1 291.1 1266.8C291.1 1240.7 269.6 1218.8 243.3 1218.8C217.2 1218.8 195.7 1240.7 195.7 1266.8C195.7 1293.1 217.2 1314.4 243.3 1314.4Z"; svg.fill "black" ]
            Svg.path [ svg.d "M335.7 1205.8C335.7 1204 337.2 1202.3 339 1202.3H381.3C416.8 1202.3 445.8 1231.2 445.8 1266.4C445.8 1302.1 416.8 1330.9 381.3 1330.9H339C337.2 1330.9 335.7 1329.2 335.7 1327.4V1205.8ZM378.7 1313.5C405.7 1313.5 425.4 1293.7 425.4 1266.5C425.4 1239.5 405.7 1219.8 378.7 1219.8H354.4V1313.5H378.7Z"; svg.fill "black" ]
            Svg.path [ svg.d "M60.4 1427.6C59.3 1425.2 60.8 1422.8 63.5 1422.8H76.5C78 1422.8 79.3 1423.9 79.6 1424.8L120 1515.8H121.1L161.5 1424.8C161.9 1423.9 163 1422.8 164.6 1422.8H177.6C180.4 1422.8 181.8 1425.2 180.7 1427.6L124.3 1551.3C123.7 1552.4 122.6 1553.3 121.2 1553.3H119.4C118.1 1553.3 116.8 1552.4 116.3 1551.3L60.4 1427.6Z"; svg.fill "black" ]
            Svg.path [ svg.d "M404.6 1421C423.2 1421 436.6 1427.2 449.1 1438.1C450.8 1439.6 450.8 1441.8 449.3 1443.2L441.2 1451.5C439.9 1453.2 438.3 1453.2 436.6 1451.5C428 1444 416.2 1439.2 404.8 1439.2C378.5 1439.2 358.7 1461.3 358.7 1487C358.7 1512.7 378.7 1534.6 405 1534.6C418.4 1534.6 427.8 1529.3 436.6 1522.5C438.3 1521.2 439.9 1521.4 441 1522.3L449.5 1530.6C451 1531.9 450.6 1534.3 449.3 1535.6C436.8 1547.7 421 1553.4 404.6 1553.4C367.8 1553.4 338.3 1524.2 338.3 1487.4C338.3 1450.6 367.9 1421 404.6 1421Z"; svg.fill "black" ]
            Svg.path [ svg.d "M458.3 1546.7L514.7 1423C515.3 1421.9 516.9 1421 517.8 1421H519.6C520.5 1421 522.2 1421.9 522.7 1423L578.8 1546.7C579.9 1549.1 578.4 1551.5 575.7 1551.5H564.1C561.9 1551.5 560.6 1550.4 559.9 1548.7L548.5 1523.5H488.4C484.7 1532 480.9 1540.2 477.2 1548.7C476.6 1550 475.2 1551.5 473 1551.5H461.4C458.7 1551.4 457.2 1549.1 458.3 1546.7ZM541.6 1507.7L519 1457.3H518.1L495.7 1507.7H541.6Z"; svg.fill "black" ]
            Svg.path [ svg.d "M589 1532.9C590.7 1530.5 592.1 1527.8 593.8 1525.4C595.5 1523 598 1522.3 600 1523.9C601.1 1524.8 615.3 1536.6 629.4 1536.6C642.1 1536.6 650.2 1528.9 650.2 1519.5C650.2 1508.5 640.6 1501.5 622.5 1494C603.8 1486.1 589.1 1476.4 589.1 1455C589.1 1440.7 600.1 1421 629.3 1421C647.7 1421 661.5 1430.6 663.3 1431.8C664.8 1432.7 666.2 1435.3 664.4 1438C662.9 1440.2 661.3 1442.8 659.8 1445C658.3 1447.4 655.9 1448.5 653.4 1446.8C652.1 1446.1 639.2 1437.6 628.6 1437.6C613.2 1437.6 607.8 1447.3 607.8 1454.1C607.8 1464.6 615.9 1471 631.1 1477.3C652.4 1485.9 670.6 1496 670.6 1518.5C670.6 1537.6 653.5 1553.2 629.6 1553.2C607.2 1553.2 593 1541.4 590.1 1538.7C588.4 1537.3 587.1 1536 589 1532.9Z"; svg.fill "black" ]
            Svg.path [ svg.d "M695.9 1426.3C695.9 1424.5 697.4 1422.8 699.4 1422.8H772.9C774.9 1422.8 776.4 1424.5 776.4 1426.3V1436.6C776.4 1438.4 774.9 1440.1 772.9 1440.1H714.8V1477.6H763.9C765.7 1477.6 767.4 1479.3 767.4 1481.1V1491.4C767.4 1493.4 765.7 1494.9 763.9 1494.9H714.8V1534.4H772.9C774.9 1534.4 776.4 1536.1 776.4 1537.9V1548C776.4 1549.8 774.9 1551.5 772.9 1551.5H699.4C697.4 1551.5 695.9 1549.8 695.9 1548V1426.3Z"; svg.fill "black" ]
            Svg.path [ svg.d "M818.1 1423.7C818.5 1422.2 819.9 1420.9 821.4 1420.9H824.3C825.4 1420.9 827.1 1421.8 827.4 1422.9L865.8 1516.8H866.5L904.7 1422.9C905.1 1421.8 906.5 1420.9 907.8 1420.9H910.7C912.2 1420.9 913.6 1422.2 914 1423.7L936.6 1547.2C937.2 1549.8 935.9 1551.4 933.3 1551.4H921.4C919.7 1551.4 918.3 1550.1 917.9 1548.8L904.3 1466.7C904.1 1466.7 903.7 1466.7 903.7 1466.7L870.6 1551.2C870.2 1552.3 869.1 1553.2 867.5 1553.2H864.2C862.7 1553.2 861.4 1552.3 861.1 1551.2L827.8 1466.7C827.6 1466.7 827.2 1466.7 827.1 1466.7L813.9 1548.8C813.7 1550.1 812.1 1551.4 810.6 1551.4H798.7C796.1 1551.4 794.8 1549.7 795.2 1547.2L818.1 1423.7Z"; svg.fill "black" ]
            Svg.path [ svg.d "M295.2 1536.5C300.447 1536.5 304.7 1532.25 304.7 1527C304.7 1521.75 300.447 1517.5 295.2 1517.5C289.953 1517.5 285.7 1521.75 285.7 1527C285.7 1532.25 289.953 1536.5 295.2 1536.5Z"; svg.fill "#ED1848" ]
            Svg.path [ svg.d "M256.6 1553C226.4 1553.6 198.6 1533.2 191.3 1502.7C182.7 1467.4 204.4 1431.7 239.8 1423.1C256.9 1419 274.6 1421.7 289.6 1430.8C304.6 1439.9 315.2 1454.4 319.4 1471.5C321.9 1481.7 321.9 1492.4 319.4 1502.6C318.1 1507.7 313 1510.8 307.9 1509.6C302.8 1508.3 299.7 1503.2 300.9 1498.1C302.7 1490.8 302.7 1483.3 300.9 1476C297.9 1463.8 290.4 1453.5 279.7 1447C269 1440.5 256.4 1438.5 244.2 1441.5C219.1 1447.6 203.6 1473 209.7 1498.1C215.8 1523.2 241.2 1538.7 266.3 1532.6C271.4 1531.3 276.6 1534.5 277.8 1539.6C279.1 1544.7 275.9 1549.8 270.8 1551.1C266.2 1552.3 261.3 1553 256.6 1553Z"; svg.fill "#ED1848" ]
            Svg.path [ svg.d "M931.4 1353.6H527C522.3 1353.6 518.6 1349.8 518.6 1345.2V1186.2C518.6 1181.5 522.4 1177.8 527 1177.8H931.4C936.1 1177.8 939.8 1181.6 939.8 1186.2V1345.2C939.9 1349.8 936.1 1353.6 931.4 1353.6Z"; svg.fill "#ED1848" ]
            Svg.path [ svg.d "M544.7 1251.9C542 1251.9 539.8 1253.4 539.8 1255.3V1272.5C539.8 1274.4 542 1275.9 544.7 1275.9C547.4 1275.9 549.6 1274.4 549.6 1272.5V1255.3C549.6 1253.5 547.4 1251.9 544.7 1251.9Z"; svg.fill "white" ]
            Svg.path [ svg.d "M569.2 1237.9C566.5 1237.9 564.3 1239.4 564.3 1241.3V1288.8C564.3 1290.7 566.5 1292.2 569.2 1292.2C571.9 1292.2 574.1 1290.7 574.1 1288.8V1241.3C574.1 1239.4 571.9 1237.9 569.2 1237.9Z"; svg.fill "white" ]
            Svg.path [ svg.d "M593.6 1214.2C590.9 1214.2 588.7 1215.7 588.7 1217.6V1313.7C588.7 1315.6 590.9 1317.1 593.6 1317.1C596.3 1317.1 598.5 1315.6 598.5 1313.7V1217.6C598.5 1215.7 596.3 1214.2 593.6 1214.2Z"; svg.fill "white" ]
            Svg.path [ svg.d "M618.1 1231.3C615.4 1231.3 613.2 1232.8 613.2 1234.7V1293.1C613.2 1295 615.4 1296.5 618.1 1296.5C620.8 1296.5 623 1295 623 1293.1V1234.7C623 1232.9 620.8 1231.3 618.1 1231.3Z"; svg.fill "white" ]
            Svg.path [ svg.d "M692 1223C689.3 1223 687.1 1224.5 687.1 1226.4V1303.7C687.1 1305.6 689.3 1307.1 692 1307.1C694.7 1307.1 696.9 1305.6 696.9 1303.7V1226.4C696.9 1224.6 694.7 1223 692 1223Z"; svg.fill "white" ]
            Svg.path [ svg.d "M740.5 1227.9C737.8 1227.9 735.6 1229.4 735.6 1231.3V1300C735.6 1301.9 737.8 1303.4 740.5 1303.4C743.2 1303.4 745.4 1301.9 745.4 1300V1231.3C745.4 1229.4 743.2 1227.9 740.5 1227.9Z"; svg.fill "white" ]
            Svg.path [ svg.d "M764.9 1245.1C762.2 1245.1 760 1246.6 760 1248.5V1282.8C760 1284.7 762.2 1286.2 764.9 1286.2C767.6 1286.2 769.8 1284.7 769.8 1282.8V1248.5C769.8 1246.6 767.6 1245.1 764.9 1245.1Z"; svg.fill "white" ]
            Svg.path [ svg.d "M716 1200.4C713.3 1200.4 711.1 1201.9 711.1 1203.8V1327.4C711.1 1329.3 713.3 1330.8 716 1330.8C718.7 1330.8 720.9 1329.3 720.9 1327.4V1203.8C720.9 1202 718.7 1200.4 716 1200.4Z"; svg.fill "white" ]
            Svg.path [ svg.d "M642.6 1255.4C639.9 1255.4 637.7 1256.9 637.7 1258.8V1272.5C637.7 1274.4 639.9 1275.9 642.6 1275.9C645.3 1275.9 647.5 1274.4 647.5 1272.5V1258.8C647.5 1256.9 645.3 1255.4 642.6 1255.4Z"; svg.fill "white" ]
            Svg.path [ svg.d "M789.4 1254.2C786.7 1254.2 784.5 1255.7 784.5 1257.6V1272.5C784.5 1274.4 786.7 1275.9 789.4 1275.9C792.1 1275.9 794.3 1274.4 794.3 1272.5V1257.6C794.3 1255.8 792.1 1254.2 789.4 1254.2Z"; svg.fill "white" ]
            Svg.path [ svg.d "M666.6 1247.7C663.9 1247.7 661.7 1249.2 661.7 1251.1V1279.1C661.7 1281 663.9 1282.5 666.6 1282.5C669.3 1282.5 671.5 1281 671.5 1279.1V1251.1C671.5 1249.2 669.3 1247.7 666.6 1247.7Z"; svg.fill "white" ]
            Svg.path [ svg.d "M815.9 1253.8C813.2 1253.8 811 1255.3 811 1257.2V1274.4C811 1276.3 813.2 1277.8 815.9 1277.8C818.6 1277.8 820.8 1276.3 820.8 1274.4V1257.2C820.8 1255.3 818.6 1253.8 815.9 1253.8Z"; svg.fill "white" ]
            Svg.path [ svg.d "M840.4 1239.7C837.7 1239.7 835.5 1241.2 835.5 1243.1V1290.6C835.5 1292.5 837.7 1294 840.4 1294C843.1 1294 845.3 1292.5 845.3 1290.6V1243.1C845.3 1241.3 843.1 1239.7 840.4 1239.7Z"; svg.fill "white" ]
            Svg.path [ svg.d "M864.9 1216C862.2 1216 860 1217.5 860 1219.4V1315.5C860 1317.4 862.2 1318.9 864.9 1318.9C867.6 1318.9 869.8 1317.4 869.8 1315.5V1219.4C869.7 1217.6 867.6 1216 864.9 1216Z"; svg.fill "white" ]
            Svg.path [ svg.d "M889.3 1233.2C886.6 1233.2 884.4 1234.7 884.4 1236.6V1295C884.4 1296.9 886.6 1298.4 889.3 1298.4C892 1298.4 894.2 1296.9 894.2 1295V1236.6C894.2 1234.7 892 1233.2 889.3 1233.2Z"; svg.fill "white" ]
            Svg.path [ svg.d "M913.8 1257.2C911.1 1257.2 908.9 1258.7 908.9 1260.6V1274.3C908.9 1276.2 911.1 1277.7 913.8 1277.7C916.5 1277.7 918.7 1276.2 918.7 1274.3V1260.6C918.7 1258.8 916.5 1257.2 913.8 1257.2Z"; svg.fill "white" ]
        ]
    ]

let podcastBtn link (svg:string) (name:string) =
    Html.a [
        prop.className $"btn btn-{svg}"
        prop.href link
        prop.children [
            Html.divClassed "w-8 h-8" [
                Html.img [
                    prop.src $"/svg/{svg}.svg"
                ]
            ]
            Html.classed Html.span "ml-2" [ Html.text name ]
        ]
    ]

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])

    Html.divClassed "flex flex-col min-h-screen" [
        Html.headerClassed "relative bg-hero py-8 sm:py-16" [
            Html.divClassed "flex flex-col items-start justify-center h-full text-default max-w-screen-xl px-8 md:px-16 lg:px-32" [
                Html.divClassed "mb-8" [ logo ]
                Html.classed Html.h1 "text-4xl font-semibold leading-none mb-4 sm:text-7xl md:text-8xl" [ Html.text "IT podcast, který rozhodně neplave po povrchu." ]
                Html.divClassed "text-xl leading-tight mb-12 sm:text-2xl md:text-3xl" [
                    Html.text "Zajímaví hosté v hodinovém pořadu vysílaném "
                    Html.a [ prop.className "text-blue-800 hover:underline"; prop.href "https://twitter.com/podvocasem"; prop.text "#PodVocasem" ]
                ]
                Html.divClassed "mb-2 text-lg md:text-xl" [ Html.text "Poslouchej nás na své oblíbené platformě:" ]
                Html.divClassed "sm:flex items-center" [
                    podcastBtn "https://open.spotify.com/show/280aceAx85AKZslVytXsrB?si=50b87e50890746b7" "spotify" "Spotify"
                    podcastBtn "https://podcasts.apple.com/us/podcast/podvocasem/id1590431276" "apple-podcast" "Apple Podcasts"
                    podcastBtn "https://podcasts.google.com/feed/aHR0cHM6Ly9mZWVkLnBvZHZvY2FzZW0uY3ovcnNz" "google-podcast" "Google Podcasts"
                ]
            ]
        ]
    ]

//    Html.button [
//        //color.isPrimary
//        prop.className "btn"
//        prop.text state.Message
//        prop.onClick (fun _ -> AskForMessage |> dispatch)
//    ]