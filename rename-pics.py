# Pythono3 code to rename multiple  
# files in a directory or folder 
  
# importing os module 
import os 
  
# Function to rename multiple files 
def main(): 
    folder = 'Testing'
    for count, filename in enumerate(os.listdir(folder)): 
        print(filename)
        src = folder + '/' + filename 
        dst = folder + '/' + folder + '-' + countStringWithZeros(count) + ".jpg"
          
        # rename() function will 
        # rename all the files 
        os.rename(src, dst) 

def countStringWithZeros(count):
    if count < 10:
        return '00' + str(count)
    if count < 100:
        return '0' + str(count)
    return str(count)

# Driver Code 
if __name__ == '__main__': 
      
    # Calling main() function 
    main() 